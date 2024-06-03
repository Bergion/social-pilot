import axios from 'axios';
import { APP_URLS, API_URLS } from '@/constants/auth';

const apiUrl = `${window.location.origin === APP_URLS.PROD ? API_URLS.PROD : API_URLS.DEV}/media`;

const generateHeaders = () => {
	// const userStore = useUserStore();
	return {
		// Authorization: `Bearer ${userStore.currentUser.access_token}`,
		'Content-Type': 'application/json',
	};
};

export const getPresignedURLs = async (filenames: string[]) => {
	try {
		const response = await axios.post(`${apiUrl}/upload`,
			filenames,
			{
				headers: generateHeaders(),				
			}
		);
		return response.data;
	} catch (err: any) {
		console.log(err)
		if (err.response?.status == 401) {
		// userNotAuthorized();
			console.log("Unauthorized");
		}
		return [];
	}
};