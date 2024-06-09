<script setup lang="ts">
import { ref, type Ref } from 'vue'
import FilePreview from '@/components/FilePreview.vue'
import { getPresignedURLs } from '@/api'

const emit = defineEmits(['uploaded']);

const files: any = ref([]);
const isDragging = ref(false);
const fileInput: any = ref(null);
const loading = ref(false);

async function onChange() {
	loading.value = true;
	files.value = [...fileInput.value.files];
	try {
		const presignedURLs = await getPresignedURLs(files.value.map((f: any)=> f.name));
		const promises: Promise<string>[] = [];
		files.value.forEach((f: any) => {
			promises.push(uploadFile(presignedURLs[f.name].URL, f))
		});
		const urls = await Promise.all(promises);

		emit('uploaded', urls.map(u => u.split('?')[0]));
	} catch (err) {
		console.log(err);
	}

	loading.value = false;
}

async function uploadFile(uploadURL: string, file: any):Promise<string> {
	const result = await fetch(uploadURL, {
 		method: 'PUT',
		body: file
	});
	return result.url;
}

function remove(i: number) {
	files.value.splice(i, 1);
}

function dragover(e: any) {
	e.preventDefault();
	isDragging.value = true;
}

function dragleave() {
	isDragging.value = false;
}

function drop(e: any) {
	e.preventDefault();
	fileInput.value.files = e.dataTransfer.files;
	onChange();
	isDragging.value = false;
}

</script>

<template>
	<div>
		<v-container fluid
      class="dropzone-container"
			:style="isDragging && 'border-color: green;'"
      @dragover="dragover"
      @dragleave="dragleave"
      @drop="drop"
    >
			<v-row no-gutters>
				<v-col class="flex">
					<input
						type="file"
						multiple
						name="file"
						id="fileInput"
						class="hidden-input"
						@change="onChange"
						ref="fileInput"
						accept=".pdf,.jpg,.jpeg,.png"
					/>
					<label for="fileInput" class="file-label">
						<div v-if="isDragging">Release to drop files here</div>
						<div v-else>Drop files here or <u>click here</u> to upload</div>
					</label>
				</v-col>
			</v-row>
			<v-row>
        <v-col v-for="(file, index) in files" :key="file.name" :cols="4">
          <FilePreview 
						:file="file"
						:loading="loading"
						@remove="remove(index)">
					</FilePreview>
        </v-col>
			</v-row>
    </v-container>
	</div>
</template>
<style>

.dropzone-container {
  background: #f7fafc;
  border: 1px dashed;
  border-color: #9e9e9e;
}
.hidden-input {
  opacity: 0;
  overflow: hidden;
  position: absolute;
  width: 1px;
  height: 1px;
}
.file-label {
  font-size: 15px;
  display: block;
  cursor: pointer;
}

.preview-card {
  display: flex;
  padding: 5px;
  margin-left: 5px;
}
.preview-img {
  width: 50px;
  height: 50px;
  border-radius: 5px;
  border: 1px solid #a2a2a2;
  background-color: #a2a2a2;
}
</style>