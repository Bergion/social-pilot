package publishers

type Publisher interface {
	Publish() []error
}
