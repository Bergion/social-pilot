<script setup>
import { computed } from 'vue';
import { watch } from 'vue';
import { ref } from 'vue'

const model = defineModel({ default: [] })

const platforms = ref([
	{
		id: 1,
		name: "Instagram",
		icon: "mdi-instagram",
		postType: "Post"
	},
	{
		id: 2,
		name: "Facebook",
		icon: "mdi-facebook"
	},
])

const platformSelection = ref([])

const instagramPostTypes = ref([
	"Post",
	"Reels",
	"Story",
])
const instagramPostTypeSelection = ref("Post")

watch(platformSelection, (value, oldValue) => {
	model.value = value.map(p => ({
		id: p.id,
		name: p.name,
		postType: p.postType
	}));
})

watch(instagramPostTypeSelection, (value) => {
	const copy = [...model.value];
	const index = copy.findIndex(p => p.id === 1);
	if (index !== -1) {
		copy[index].postType = value;
		model.value = copy
	}
})

const instagramSelected = computed(() => {
	return !!platformSelection.value.filter(p => p.id == 1).length
})
</script>

<template>
	<v-item-group
		v-model="platformSelection" 
		multiple 
		class="mt-8"
	>
		<v-container>
			<v-row>
				<p class="font-bold mb-4">Select platform</p>
			</v-row>
			<v-row>
				<v-item v-for="platform in platforms"
					:key="platform.id"
					:value="platform"
					v-slot="{ isSelected, toggle }">
					<v-btn
						:color="isSelected ? 'green' : ''"
						class="d-flex align-center w-12 mr-4"
						variant="outlined"
						@click="toggle"
					>
						<v-icon size="x-large">
							{{ platform.icon }}
						</v-icon>
					</v-btn>
				</v-item>
			</v-row>
			<v-row 
				v-if="instagramSelected"
				class="mt-8"
			>
				<v-select
					v-model="instagramPostTypeSelection"
					:items="instagramPostTypes"
					variant="outlined"
					label="Post type"
				></v-select>
			</v-row>
		</v-container>
	</v-item-group>
</template>