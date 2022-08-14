package heast.client.view.utility

import javafx.scene.layout.HBox
import javafx.scene.layout.Priority
import javafx.scene.layout.Region
import javafx.scene.layout.VBox

class FlexExpander(hBox: Boolean = false, vBox: Boolean = false) : Region() {
	init {
		if (hBox) {
			HBox.setHgrow(this, Priority.ALWAYS)
		}
		if (vBox) {
			VBox.setVgrow(this, Priority.ALWAYS)
		}
	}
}