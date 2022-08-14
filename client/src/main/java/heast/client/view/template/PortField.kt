package heast.client.view.template

class PortField(prompt: String = "") : TextField(prompt) {
	init {
		this.textProperty().addListener { _, _, v ->
			val str = v.replace("\\D".toRegex(), "")
			this.text = str
			if (str.isNotBlank()) {
				this.text = if (str.toInt() !in 0..65535) "65535" else str
			}
		}
	}
}