package models

import (
	"context"
	"errors"
	"flag"
	"log"
	"path"
	"strings"

	pb "github.com/JesusDeLaProg/new-timesheet-manager/proto"

	"cloud.google.com/go/firestore"
	firebase "firebase.google.com/go"
	"google.golang.org/api/option"
)

var service_account_credentials_path = flag.String("service_account_credentials_path", "", "Path of the service account credentials to use when connecting to Firebase.")

var firebase_environment = flag.String("firebase_environment", "development", "Environment that should be used in the Firebase Database. Environments will be created as needed as a top-level document in the Database.")

type FirebaseModel[T any] struct {
	Model[T]
	collection *firestore.CollectionRef
}

func (m *FirebaseModel[T]) GetById(id Id) *T {
	return nil
}

func (m *FirebaseModel[T]) Get(opts pb.FetchOptions, fields []string) []*T {
	return nil
}

func (m *FirebaseModel[T]) Create(t *T) (Id, error) {
	return make(Id, 0), nil
}

func (m *FirebaseModel[T]) Update(t *T) error {
	return nil
}

func (m *FirebaseModel[T]) Delete(id Id) error {
	return nil
}

func GetModelsForFirebase(dbAddr string) (*Models, error) {
	parts := strings.Split(dbAddr, ":")
	if len(parts) != 2 {
		return nil, errors.New("dbAddr should be a 2-part string separated with `:` where the first part is the project name and the second part is the top-level collection name")
	}
	if *service_account_credentials_path == "" {
		panic("--service_account_credentials_path must be set")
	}
	clean_path := path.Clean(*service_account_credentials_path)
	sa := option.WithCredentialsFile(clean_path)
	ctx := context.Background()
	app, err := firebase.NewApp(ctx, nil, sa)
	if err != nil {
		log.Fatalln(err)
	}

	client, err := app.Firestore(ctx)
	if err != nil {
		log.Fatalln(err)
	}
	db := client.Collection(parts[1])
	root := db.Doc(*firebase_environment)
	return &Models{
		Activity:  &FirebaseModel[pb.Activity]{collection: root.Collection("activity")},
		Phase:     &FirebaseModel[pb.Phase]{collection: root.Collection("phase")},
		Client:    &FirebaseModel[pb.Client]{collection: root.Collection("client")},
		Project:   &FirebaseModel[pb.Project]{collection: root.Collection("project")},
		Timesheet: &FirebaseModel[pb.Timesheet]{collection: root.Collection("timesheet")},
		Employee:  &FirebaseModel[pb.Employee]{collection: root.Collection("employee")},
		User:      &FirebaseModel[pb.User]{collection: root.Collection("user")},
	}, nil
}
