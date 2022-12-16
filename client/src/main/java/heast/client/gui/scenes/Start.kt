package heast.client.gui.scenes

import heast.client.gui.components.window.Default
import heast.client.gui.components.window.Title
import heast.client.gui.components.window.WindowHeight
import heast.client.gui.registry.Icons
import heast.client.gui.registry.Icons.toImg
import javafx.scene.Node
import javafx.scene.Parent
import javafx.scene.image.ImageView
import kotlin.reflect.KClass

@WindowHeight(520)
object Start : Default() {
	override val back : KClass<out Parent>?
		get() = null

	override val forward : KClass<out Parent>
		get() = Welcome::class

	override val title : Node
		get() = Title()

	override val layout : Node
		get() = ImageView(Icons.Logo.BIG.toImg()).apply {
			this.fitWidth = 240.0
			this.fitHeight = 240.0
		}
}
