<script setup>
import { ref } from 'vue'

const platforms = ref([
	{
		id: 1,
		icon: "mdi-instagram"
	},
	{
		id: 2,
		icon: "mdi-facebook"
	},
])
const selection = ref([])
</script>

<template>
	<v-item-group
		v-model="selection" 
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
		</v-container>
	</v-item-group>
</template>