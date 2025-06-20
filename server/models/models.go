package models

import (
	"strings"

	pb "github.com/JesusDeLaProg/new-timesheet-manager/proto"
)

type Id []byte

type Model[T any] interface {
	GetById(id Id) *T
	Get(opts pb.FetchOptions, fields []string) []*T
	Create(t *T) (Id, error)
	Update(t *T) error
	Delete(id Id) error
}

type ActivityModel interface {
	Model[pb.Activity]
}
type PhaseModel interface {
	Model[pb.Phase]
}
type ClientModel interface {
	Model[pb.Client]
}
type ProjectModel interface {
	Model[pb.Project]
}
type TimesheetModel interface {
	Model[pb.Timesheet]
}
type EmployeeModel interface {
	Model[pb.Employee]
}
type UserModel interface {
	Model[pb.User]
}

type Models struct {
	Activity  ActivityModel
	Phase     PhaseModel
	Client    ClientModel
	Project   ProjectModel
	Timesheet TimesheetModel
	Employee  EmployeeModel
	User      UserModel
}

func GetModelsForDb(dbAddr string) (*Models, error) {
	if strings.HasPrefix(dbAddr, "firebase:") {
		return GetModelsForFirebase(strings.TrimPrefix(dbAddr, "firebase:"))
	}
	return &Models{
		Activity:  nil,
		Phase:     nil,
		Client:    nil,
		Project:   nil,
		Timesheet: nil,
		Employee:  nil,
		User:      nil,
	}, nil
}
