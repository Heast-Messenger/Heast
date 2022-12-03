package heast.client.gui.utility

object TextExtension {
	fun String.toText() = javafx.scene.control.Label(this)
}