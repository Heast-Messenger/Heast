package heast.client.view.template

import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.scene.layout.Background
import javafx.scene.layout.BackgroundFill
import javafx.scene.layout.BorderPane
import javafx.scene.layout.CornerRadii
import heast.client.model.Settings

open class ViewPane : BorderPane() {
	init {
		this.padding = Insets(10.0)
		this.backgroundProperty().bind(
			Bindings.createObjectBinding({
				Background(
					BackgroundFill(
						Settings.colors["Secondary Color"]!!.color.value,
						CornerRadii(10.0),
						Insets.EMPTY
					)
				)
			}, Settings.colors["Secondary Color"]!!.color)
		)
	}
}