package services

import (
	"context"

	pb "github.com/JesusDeLaProg/new-timesheet-manager/proto"
	"github.com/JesusDeLaProg/new-timesheet-manager/server/models"
	"google.golang.org/grpc"
	"google.golang.org/grpc/codes"
	"google.golang.org/grpc/reflection"
	"google.golang.org/grpc/status"
)

type ActivityServer struct {
	models    *models.Models
	validator Validator[pb.Activity]
}
type PhaseServer struct {
	models    *models.Models
	validator Validator[pb.Phase]
}
type ClientServer struct {
	models    *models.Models
	validator Validator[pb.Client]
}
type ProjectServer struct {
	models    *models.Models
	validator Validator[pb.Project]
}
type TimesheetServer struct {
	models    *models.Models
	validator Validator[pb.Timesheet]
}
type EmployeeServer struct {
	models    *models.Models
	validator Validator[pb.Employee]
}
type UserServer struct {
	models    *models.Models
	validator Validator[pb.User]
}

func (s *ActivityServer) Fetch(ctx context.Context, r *pb.FetchActivitiesRequest) (*pb.FetchActivitiesResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ActivityServer) Create(ctx context.Context, r *pb.CreateActivityRequest) (*pb.CreateActivityResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ActivityServer) Update(ctx context.Context, r *pb.UpdateActivityRequest) (*pb.UpdateActivityResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ActivityServer) Delete(ctx context.Context, r *pb.DeleteActivityRequest) (*pb.DeleteActivityResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ActivityServer) Validate(ctx context.Context, r *pb.ValidateActivityRequest) (*pb.ValidateActivityResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *PhaseServer) Fetch(ctx context.Context, r *pb.FetchPhasesRequest) (*pb.FetchPhasesResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *PhaseServer) Create(ctx context.Context, r *pb.CreatePhaseRequest) (*pb.CreatePhaseResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *PhaseServer) Update(ctx context.Context, r *pb.UpdatePhaseRequest) (*pb.UpdatePhaseResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *PhaseServer) Delete(ctx context.Context, r *pb.DeletePhaseRequest) (*pb.DeletePhaseResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *PhaseServer) Validate(ctx context.Context, r *pb.ValidatePhaseRequest) (*pb.ValidatePhaseResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ClientServer) Fetch(ctx context.Context, r *pb.FetchClientsRequest) (*pb.FetchClientsResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ClientServer) Create(ctx context.Context, r *pb.CreateClientRequest) (*pb.CreateClientResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ClientServer) Update(ctx context.Context, r *pb.UpdateClientRequest) (*pb.UpdateClientResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ClientServer) Delete(ctx context.Context, r *pb.DeleteClientRequest) (*pb.DeleteClientResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ClientServer) Validate(ctx context.Context, r *pb.ValidateClientRequest) (*pb.ValidateClientResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ProjectServer) Fetch(ctx context.Context, r *pb.FetchProjectsRequest) (*pb.FetchProjectsResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ProjectServer) Create(ctx context.Context, r *pb.CreateProjectRequest) (*pb.CreateProjectResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ProjectServer) Update(ctx context.Context, r *pb.UpdateProjectRequest) (*pb.UpdateProjectResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ProjectServer) Delete(ctx context.Context, r *pb.DeleteProjectRequest) (*pb.DeleteProjectResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *ProjectServer) Validate(ctx context.Context, r *pb.ValidateProjectRequest) (*pb.ValidateProjectResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *TimesheetServer) Fetch(ctx context.Context, r *pb.FetchTimesheetsRequest) (*pb.FetchTimesheetsResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *TimesheetServer) Create(ctx context.Context, r *pb.CreateTimesheetRequest) (*pb.CreateTimesheetResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *TimesheetServer) Update(ctx context.Context, r *pb.UpdateTimesheetRequest) (*pb.UpdateTimesheetResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *TimesheetServer) Delete(ctx context.Context, r *pb.DeleteTimesheetRequest) (*pb.DeleteTimesheetResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *TimesheetServer) Validate(ctx context.Context, r *pb.ValidateTimesheetRequest) (*pb.ValidateTimesheetResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *EmployeeServer) Fetch(ctx context.Context, r *pb.FetchEmployeesRequest) (*pb.FetchEmployeesResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *EmployeeServer) Create(ctx context.Context, r *pb.CreateEmployeeRequest) (*pb.CreateEmployeeResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *EmployeeServer) Update(ctx context.Context, r *pb.UpdateEmployeeRequest) (*pb.UpdateEmployeeResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *EmployeeServer) Delete(ctx context.Context, r *pb.DeleteEmployeeRequest) (*pb.DeleteEmployeeResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *EmployeeServer) Validate(ctx context.Context, r *pb.ValidateEmployeeRequest) (*pb.ValidateEmployeeResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *UserServer) Fetch(ctx context.Context, r *pb.FetchUsersRequest) (*pb.FetchUsersResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *UserServer) Create(ctx context.Context, r *pb.CreateUserRequest) (*pb.CreateUserResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *UserServer) Update(ctx context.Context, r *pb.UpdateUserRequest) (*pb.UpdateUserResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *UserServer) Delete(ctx context.Context, r *pb.DeleteUserRequest) (*pb.DeleteUserResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *UserServer) Validate(ctx context.Context, r *pb.ValidateUserRequest) (*pb.ValidateUserResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func (s *UserServer) FetchUsernames(ctx context.Context, r *pb.FetchUsernamesRequest) (*pb.FetchUsernamesResponse, error) {
	return nil, status.Error(codes.Unimplemented, "Method unimplemented")
}

func RegisterServices(enable_reflection bool, s *grpc.Server, m *models.Models) {
	pb.RegisterActivityServiceServer(s, &ActivityServer{models: m, validator: &ActivityValidator{}})
	pb.RegisterPhaseServiceServer(s, &PhaseServer{models: m, validator: &PhaseValidator{}})
	pb.RegisterClientServiceServer(s, &ClientServer{models: m, validator: &ClientValidator{}})
	pb.RegisterProjectServiceServer(s, &ProjectServer{models: m, validator: &ProjectValidator{}})
	pb.RegisterTimesheetServiceServer(s, &TimesheetServer{models: m, validator: &TimesheetValidator{}})
	pb.RegisterEmployeeServiceServer(s, &EmployeeServer{models: m, validator: &EmployeeValidator{}})
	pb.RegisterUserServiceServer(s, &UserServer{models: m, validator: &UserValidator{}})
	if enable_reflection {
		reflection.Register(s)
	}
}
