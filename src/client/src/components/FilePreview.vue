<script setup lang="ts">
const props = defineProps({
	file: Object,
	loading: Boolean,
});

function generateThumbnail(file: any) {
	let fileSrc = URL.createObjectURL(file);
	setTimeout(() => {
		URL.revokeObjectURL(fileSrc);
	}, 1000);
	return fileSrc;
}

</script>

<template>
  <v-card>
		<div class="absolute right-0 px-3 z-50">
			<button
				type="button"
				title="Remove file"
				@click="$emit('remove')"
			>
				<b class="text-2xl">&times;</b>
			</button>
		</div>
		<v-img
			:lazy-src="generateThumbnail(props.file)"
			:src="generateThumbnail(props.file)"
			class="align-end"
			height="320"
			aspect-ratio="16/9"
			contain
		>
			<template v-slot:placeholder>
				<div v-if="loading" class="d-flex align-center justify-center fill-height">
					<v-progress-circular
						color="grey-lighten-4"
						indeterminate
					></v-progress-circular>
				</div>
			</template>
		</v-img>
	</v-card>
</template>
