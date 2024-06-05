interface IPost {
  media: IMedia[]
	text: string
	status: number
	scheduledFor: Date
	platforms: IPlatform[]
}

interface IPlatform {
	id: number
	name: string
	postType: string
}

interface IMedia {
	url: string
}