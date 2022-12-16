package heast.client.gui.cssengine

class Align(private val align: String) : CSSProperty() {
	companion object {
		val topLeft = Align("top-left")
		val topCenter = Align("top-center")
		val topRight = Align("top-right")
		val centerLeft = Align("center-left")
		val center = Align("center")
		val centerRight = Align("center-right")
		val bottomLeft = Align("bottom-left")
		val bottomCenter = Align("bottom-center")
		val bottomRight = Align("bottom-right")
	}

	override fun toString() =
		"-fx-alignment: $align;"
}