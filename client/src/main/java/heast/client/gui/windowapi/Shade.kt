package heast.client.gui.windowapi

import javafx.scene.paint.Color
import javafx.scene.shape.Rectangle

class Shade : Rectangle() {
	init {
		this.fill = Color.BLACK
		this.isPickOnBounds = false
	}

	fun setValue(percent: Double) {
		this.opacity = percent / 0.5
	}
}