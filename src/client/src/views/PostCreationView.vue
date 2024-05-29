<script setup lang="ts">
import FileUploader from '@/components/FileUploader.vue'
</script>

<template>
  <v-container class="bg-white w-full" fluid>
		<v-row>
			<v-col cols="12" md="6">
        <p class="text-3xl font-extrabold">Create new post</p>
      </v-col>
		</v-row>
		<v-row>
			<v-col cols="12" md="6">
				<FileUploader></FileUploader>
			</v-col>
		</v-row>
	</v-container>
</template>