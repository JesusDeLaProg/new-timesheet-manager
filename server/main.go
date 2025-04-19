package main

import (
	"flag"
	"fmt"
	"log"
	"net"

	_ "github.com/JesusDeLaProg/timesheet-manager/proto"
	"google.golang.org/grpc"
	"google.golang.org/grpc/reflection"
)

var port = flag.Int("port", 8080, "The server port")

// type greetingServer struct {
// 	pb.UnimplementedGreetingServer
// }
//
// type emptyNameError struct{}
//
// func (emptyNameError) Error() string {
// 	return "empty name"
// }
//
// func (s *greetingServer) Greet(ctx context.Context, in *pb.GreetRequest) (*pb.GreetResponse, error) {
// 	if in.GetName() == "" {
// 		return nil, emptyNameError{}
// 	}
// 	message := fmt.Sprintf("Hello, %s!", in.GetName())
// 	out := &pb.GreetResponse{
// 		Message: &message,
// 	}
// 	return out, nil
// }

func main() {
	flag.Parse()
	lis, err := net.Listen("tcp", fmt.Sprintf(":%d", *port))
	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}
	grpcServer := grpc.NewServer()
	// pb.RegisterGreetingServer(grpcServer, &greetingServer{})
	reflection.Register(grpcServer)
	if err := grpcServer.Serve(lis); err != nil {
		log.Fatalf("failed to serve: %v", err)
	}
	log.Printf("server listening at %v", lis.Addr())
	grpcServer.Serve(lis)
}
