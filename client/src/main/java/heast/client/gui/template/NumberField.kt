package heast.client.gui.template

open class NumberField(prompt: String = "") : TextField(prompt) {
	init {
		this.textProperty().addListener { _, _, v ->
			if (v.isNotBlank()) {
				this.text = v.replace(Regex("\\D"), "")
			}
		}
	}
}