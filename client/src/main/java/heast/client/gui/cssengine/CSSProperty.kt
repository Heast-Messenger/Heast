package heast.client.gui.cssengine

import javafx.scene.Node

abstract class CSSProperty {
	abstract override fun toString() : String

	companion object {
		const val MULTIPLIER = 4

		var Node.css : List<CSSProperty>
			get() { throw UnsupportedOperationException() } // add css decoder
			set(value) { this.style = value.joinToString("") }

		fun Node.removeCSSProperty(vararg properties : String) {
			for (property in properties) {
				this.style = this.style.replace("${property}:.*?;".toRegex(), "")
			}
		}
	}
}