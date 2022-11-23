package heast.client.gui.utility

import javafx.scene.paint.Color

object ColorExtension {
	fun Color.toAWT(): java.awt.Color {
		return java.awt.Color(
			this.red.toFloat(),
			this.green.toFloat(),
			this.blue.toFloat(),
			this.opacity.toFloat())
	}
}