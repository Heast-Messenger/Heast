package heast.client.view.utility

import javafx.scene.paint.Color
import javafx.scene.shape.Rectangle

open class FlexSpacer(size: Double, hBox: Boolean = false, vBox: Boolean = false) : Rectangle() {
	init {
		this.fill = Color.TRANSPARENT
		if (hBox) {
			this.width = size
		}
		if (vBox) {
			this.height = size
		}
	}
}