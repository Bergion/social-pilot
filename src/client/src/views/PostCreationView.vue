<script setup lang="ts">
import FileUploader from '@/components/FileUploader.vue'
import PlatformPicker from '@/components/PlatformPicker.vue' 
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import { ref } from 'vue';
import { createPost } from '@/api'
import { watch } from 'vue';

const urls = ref<string[]>([])
const text = ref("")
const date = ref()
const platforms = ref([])

async function post() {
	try {
		await createPost({
			text: text.value,
			media: urls.value.map(u => ({url: u})),
			scheduledFor: date.value,
			status: 2,
			platforms: platforms.value
		});
	} catch (err: any) {
		console.log(err)
	}
}

function schedule() {

}

function onUploaded(e: string[]) {
	urls.value = e
}

</script>

<template>
  <v-container class="bg-white w-full" fluid>
		<v-row>
			<v-col cols="12" sm="6">
        <p class="text-3xl font-extrabold">Create new post</p>
				<PlatformPicker v-model="platforms"></PlatformPicker>
				<FileUploader 
					class="mt-12"
					@uploaded="onUploaded"
				>
				</FileUploader>
				<v-textarea
          class="mt-4"
					row-heigh="25"
					variant="outlined"
          rows="3"
					auto-grow
					shaped
					v-model="text"
        ></v-textarea>
				<div class="flex justify-end">
					<v-btn
						class="mr-2"
						color="primary"
						prepend-icon="mdi-calendar"
					>
						Schedule
					</v-btn>
					<v-btn
						color="primary"
						@click="post"
					>
						Post
					</v-btn>
				</div>
      </v-col>
			<v-col md="6 flex align-center justify-center">
				<VueDatePicker v-model="date" inline auto-apply />
			</v-col>
		</v-row>
	</v-container>
</template>
<style>
.dp__main {
	width: auto;
	border: none
}

.dp__menu {
	border: none
}

</style>