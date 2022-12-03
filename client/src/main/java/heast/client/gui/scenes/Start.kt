package heast.client.gui.scenes

import heast.client.gui.components.layout.Pad
import heast.client.gui.components.welcome.*
import heast.client.gui.registry.Icons
import heast.client.gui.registry.Icons.toImg
import javafx.scene.Parent
import javafx.scene.image.ImageView
import javafx.scene.layout.*
import kotlin.reflect.KClass

object Start : Default() {
	init {
		this.top = HBox(
			Pad.vbox(52.0)
		)

		this.center = VBox().apply {
			this.styleClass.addAll(
				"spacing-3",
				"align-top-center",
				"px-16")
			this.children.addAll(
				Pad.vbox(16.0),
				Title(),
				ImageView(Icons.logo["big"]!!.toImg()).apply {
					this.fitWidth = 280.0
					this.fitHeight = 280.0
				})
		}

		this.bottom = HBox().apply {
			this.styleClass.addAll(
				"align-center")
			this.children.addAll(
				Navigator(Icons.menu["next"]!!, Welcome::class))
		}
	}

	override val back : KClass<out Parent>?
		get() = null
}
