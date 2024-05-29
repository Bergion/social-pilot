<script setup>
import { ref } from 'vue'
import TikTokIcon from '@/components/TikTokIcon.vue'

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
					<v-item v-slot="{ isSelected, toggle }">
						<v-btn
							:color="isSelected ? 'green' : ''"
							class="d-flex align-center w-12"
							variant="outlined"
							@click="toggle"
						>
							<v-icon size="x-large">
								mdi-instagram
							</v-icon>
						</v-btn>
					</v-item>
					<v-item v-slot="{ isSelected, toggle }">
						<v-btn
							variant="outlined"
							:color="isSelected ? 'green' : ''"
							class="d-flex align-center ml-4"
							dark
							@click="toggle"
						>
              <TikTokIcon class="w-6"></TikTokIcon>
            </v-btn>
					</v-item>
			</v-row>
		</v-container>
	</v-item-group>
</template>