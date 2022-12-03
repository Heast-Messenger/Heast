package heast.client.gui.windowapi

import heast.client.gui.GuiMain
import javafx.animation.TranslateTransition
import javafx.scene.Node
import javafx.scene.layout.StackPane
import javafx.util.Duration
import java.util.*

class Mantle(content: Node) : StackPane() {

	var content: Node = content
		set(value) {
			if (this.children.contains(value)) return

			value.apply {
				this.translateX = GuiMain.window.width.toDouble()
				TranslateTransition().apply {
					this.node = value
					this.duration = Duration.millis(1000.0)
					this.fromX = GuiMain.window.width.toDouble()
					this.toX = 0.0
					this.play()
				}
			}

			field.also { current ->
				TranslateTransition().apply {
					this.node = current
					this.duration = Duration.millis(1000.0)
					this.fromX = 0.0
					this.toX = -GuiMain.window.width.toDouble()
					this.play()

					this.setOnFinished {
						this@Mantle.children.remove(current)
					}
				}
			}

			this.children.add(1, value)
			field = value
		}

	val shade: Shade

	init {
		this.children.add(0, this@Mantle.content)
		this.children.add(1, Shade().apply { this@Mantle.shade = this })
	}
}