package main

import (
	"flag"
	"fmt"
	"log"
	"net"

	"github.com/JesusDeLaProg/new-timesheet-manager/server/services"
	"google.golang.org/grpc"
)

var port = flag.Int("port", 8080, "The server port")
var enable_reflection = flag.Bool("enable_reflection", true, "If true, sets up reflection on the gRPC server.")

func main() {
	flag.Parse()
	lis, err := net.Listen("tcp", fmt.Sprintf(":%d", *port))
	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}
	grpcServer := grpc.NewServer()
	services.RegisterServices(grpcServer, *enable_reflection)
	if err := grpcServer.Serve(lis); err != nil {
		log.Fatalf("failed to serve: %v", err)
	}
	log.Printf("server listening at %v", lis.Addr())
	grpcServer.Serve(lis)
}
