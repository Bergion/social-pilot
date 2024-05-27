package constants

type Platform int

const (
	Instagram Platform = iota + 1
	Reddit
)

func (p Platform) String() string {
	return [...]string{"Instagram", "Reddit"}[p-1]
}
