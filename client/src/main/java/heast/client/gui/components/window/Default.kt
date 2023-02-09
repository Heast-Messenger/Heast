package heast.client.gui.components.window

import heast.client.gui.components.layout.Pad
import heast.client.gui.cssengine.Align
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.cssengine.Padding
import heast.client.gui.registry.Icons
import javafx.scene.Node
import javafx.scene.Parent
import javafx.scene.layout.BorderPane
import javafx.scene.layout.HBox
import kotlin.reflect.KClass

abstract class Default : BorderPane() {

	abstract val back: KClass<out Parent>?
	abstract val forward: KClass<out Parent>?

	abstract val title: Node?
	abstract val layout: Node?

	init {
		if (back != null) {
			this.top = HBox(
				Navigator(Icons.Menu.BACK, back)
			).apply {
				this.css = listOf(
					Align.centerRight,
					Padding().x(4))
			}
		} else {
			this.top = Pad.vbox(60.0)
		}

		if (forward != null) this.bottom = HBox(
			Navigator(Icons.Menu.NEXT, forward)
		).apply {
			this.css = listOf(
				Align.center,
				Padding().x(4))
		} else {
			this.bottom = Pad.vbox(40.0)
		}

		this.center = BorderPane().apply {
			this.top = title
			this.center = layout
			this.css = listOf(
				Padding().x(16))
		}
	}
}