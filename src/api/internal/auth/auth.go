package auth

import "context"

type User struct {
	Id    string
	Name  string
	Email string
}

func GetUserFromContext(ctx context.Context) (User, bool) {
	// user, ok := ctx.Value("user").(User)

	user, ok := User{Id: "1", Name: "D", Email: "dima.vintsyk@gmail.com"}, true
	return user, ok
}
