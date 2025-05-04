package services

import (
	"errors"
	"log"

	"buf.build/go/protovalidate"
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

func Validate(m proto.Message) []*pb.ValidationError {
	errs := make([]*pb.ValidationError, 0)
	err := protovalidate.Validate(m)
	var valErr *protovalidate.ValidationError
	if ok := errors.As(err, &valErr); ok {
		for i := 0; i < len(valErr.Violations); i++ {
			v := valErr.Violations[i]
			errs = append(errs, pb.ValidationError_builder{
				Path:  proto.String(protovalidate.FieldPathString(v.Proto.GetField())),
				Error: proto.String(v.Proto.GetMessage()),
			}.Build())
		}
	}
	log.Printf("Got errors %v for activity %v", errs, m)
	return errs
}

func (v *ActivityValidator) Validate(a *pb.Activity) []*pb.ValidationError {
	return Validate(a)
}

func (v *PhaseValidator) Validate(p *pb.Phase) []*pb.ValidationError {
	return Validate(p)
}

func (v *ClientValidator) Validate(c *pb.Client) []*pb.ValidationError {
	return Validate(c)
}

func (v *ProjectValidator) Validate(p *pb.Project) []*pb.ValidationError {
	return Validate(p)
}

func (v *TimesheetValidator) Validate(t *pb.Timesheet) []*pb.ValidationError {
	return Validate(t)
}

func (v *EmployeeValidator) Validate(e *pb.Employee) []*pb.ValidationError {
	return Validate(e)
}

func (v *UserValidator) Validate(u *pb.User) []*pb.ValidationError {
	return Validate(u)
}
