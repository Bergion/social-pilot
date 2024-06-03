package mediator

import (
	"context"
	"errors"
	"fmt"
	"reflect"
)

type Mediator interface {
}

type RequestHandler[Request any, Response any] interface {
	Handle(request Request, ctx context.Context) (Response, error)
}

type Unit struct{}

var requestHandlers = make(map[reflect.Type]interface{})

func RegisterHandler[Request any, Response any](handler RequestHandler[Request, Response]) error {
	var request Request
	requestType := reflect.TypeOf(request)
	_, ok := requestHandlers[requestType]
	if ok {
		return fmt.Errorf("registered handler already exists in the registry for message %s", requestType.String())
	}

	if handler == nil {
		return errors.New("request handler must not be nil")
	}

	requestHandlers[requestType] = handler

	return nil
}

func Send[Request any, Response any](request Request, ctx context.Context) (Response, error) {
	requestType := reflect.TypeOf(request)
	handlerValue, ok := requestHandlers[requestType]
	if !ok {
		return *new(Response), errors.New("no handler registered")
	}

	handler := handlerValue.(RequestHandler[Request, Response])
	response, err := handler.Handle(request, ctx)

	return response, err
}
