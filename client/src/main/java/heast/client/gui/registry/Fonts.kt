package heast.client.gui.registry

import heast.client.ClientResources
import javafx.scene.text.Font

object Fonts {
	fun init() {
		val poppinsBold = ClientResources.getResource("fonts/poppins-bold.ttf")
		val poppinsMedium = ClientResources.getResource("fonts/poppins-medium.ttf")
		val interBold = ClientResources.getResource("fonts/inter-bold.ttf")
		val interMedium = ClientResources.getResource("fonts/inter-medium.ttf")

		Font.loadFont(poppinsBold, 12.0)
		Font.loadFont(poppinsMedium, 12.0)
		Font.loadFont(interBold, 12.0)
		Font.loadFont(interMedium, 12.0)
	}
}