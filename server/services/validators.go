package services

import (
	pb "github.com/JesusDeLaProg/new-timesheet-manager/proto"
	"google.golang.org/protobuf/proto"
)

type Validator[T any] interface {
	Validate(*T) []*pb.ValidationError
}

type ActivityValidator struct{}
type PhaseValidator struct{}
type ClientValidator struct{}
type ProjectValidator struct{}
type TimesheetValidator struct{}
type EmployeeValidator struct{}
type UserValidator struct{}

func (v *ActivityValidator) Validate(a *pb.Activity) []*pb.ValidationError {
	errors := make([]*pb.ValidationError, 0)

	if len(a.GetCode()) != 2 {
		errors = append(errors, pb.ValidationError_builder{
			Path:  proto.String("code"),
			Error: proto.String("Code should have a length of 2"),
		}.Build())
	}

	return errors
}

func (v *PhaseValidator) Validate(a *pb.Phase) []*pb.ValidationError {
	return make([]*pb.ValidationError, 0)
}

func (v *ClientValidator) Validate(a *pb.Client) []*pb.ValidationError {
	return make([]*pb.ValidationError, 0)
}

func (v *ProjectValidator) Validate(a *pb.Project) []*pb.ValidationError {
	return make([]*pb.ValidationError, 0)
}

func (v *TimesheetValidator) Validate(a *pb.Timesheet) []*pb.ValidationError {
	return make([]*pb.ValidationError, 0)
}

func (v *EmployeeValidator) Validate(a *pb.Employee) []*pb.ValidationError {
	return make([]*pb.ValidationError, 0)
}

func (v *UserValidator) Validate(a *pb.User) []*pb.ValidationError {
	return make([]*pb.ValidationError, 0)
}
