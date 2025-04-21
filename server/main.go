package main

import (
	"context"
	"flag"
	"fmt"
	"log"
	"net"

	pb "github.com/JesusDeLaProg/new-timesheet-manager/proto"
	"google.golang.org/grpc"
	"google.golang.org/grpc/codes"
	"google.golang.org/grpc/reflection"
	"google.golang.org/grpc/status"
)

var port = flag.Int("port", 8080, "The server port")

type ActivityServer struct{}

func (s *ActivityServer) Fetch(ctx context.Context, r *pb.FetchActivitiesRequest) (*pb.FetchActivitiesResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Fetch is unimplemented")
}

func (s *ActivityServer) Save(ctx context.Context, r *pb.SaveActivitiesRequest) (*pb.SaveActivitiesResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Save is unimplemented")
}

func main() {
	flag.Parse()
	lis, err := net.Listen("tcp", fmt.Sprintf(":%d", *port))
	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}
	grpcServer := grpc.NewServer()
	pb.RegisterActivityServiceServer(grpcServer, &ActivityServer{})
	reflection.Register(grpcServer)
	if err := grpcServer.Serve(lis); err != nil {
		log.Fatalf("failed to serve: %v", err)
	}
	log.Printf("server listening at %v", lis.Addr())
	grpcServer.Serve(lis)
}
