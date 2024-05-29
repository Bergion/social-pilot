<script setup lang="ts">
import FileUploader from '@/components/FileUploader.vue'
import PlatformPicker from '@/components/PlatformPicker.vue' 
</script>

<template>
  <v-container class="bg-white w-full" fluid>
		<v-row>
			<v-col cols="12" sm="6">
        <p class="text-3xl font-extrabold">Create new post</p>
				<PlatformPicker></PlatformPicker>
				<FileUploader class="mt-12"></FileUploader>
      </v-col>
			<v-col md="6 flex justify-center">
				<v-date-picker></v-date-picker>
			</v-col>
		</v-row>
	</v-container>
</template>
<style>
.v-picker-title {
	display: none;
}

</style>