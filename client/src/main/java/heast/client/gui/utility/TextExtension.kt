package heast.client.gui.utility

import javafx.scene.text.Text

object TextExtension {
	fun String.toText() = Text(this)
}