package heast.client.gui.windowapi

import heast.client.gui.GuiMain
import heast.client.gui.components.window.WindowHeight
import heast.client.gui.registry.Interpolators
import javafx.animation.TranslateTransition
import javafx.scene.Node
import javafx.scene.layout.StackPane
import javafx.util.Duration
import kotlin.reflect.full.findAnnotation

class Mantle(content: Node) : StackPane() {

	companion object {
		private const val ANIMATE = false
	}

	var content : Node = content
		set(value) {
			if (this.children.contains(value)) return

			GuiMain.window.setHeight(
				value::class.findAnnotation<WindowHeight>()?.value ?: 500)

			if (ANIMATE) {
				this.children.add(1, value)

				value.apply {
					this.translateX = GuiMain.window.width.toDouble()
					TranslateTransition().apply {
						this.node = value
						this.duration = Duration.millis(900.0)
						this.fromX = GuiMain.window.width.toDouble()
						this.toX = 0.0
						this.interpolator = Interpolators.CUBIC
						this.play()
					}
				}

				field.also { current ->
					TranslateTransition().apply {
						this.node = current
						this.duration = Duration.millis(900.0)
						this.fromX = 0.0
						this.toX = -GuiMain.window.width.toDouble()
						this.interpolator = Interpolators.CUBIC
						this.play()

						this.setOnFinished {
							this@Mantle.children.remove(current)
						}
					}
				}
			} else {
				this.children.remove(field)
				this.children.add(1, value)
			}

			field = value
		}

	val shade : Shade

	init {
		this.children.add(0, this@Mantle.content)
		this.children.add(1, Shade().apply { this@Mantle.shade = this })
	}

	fun setShadeOpacity(opacity : Double) {
		this.shade.setValue(opacity)
	}
}