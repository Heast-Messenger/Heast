package heast.client.view

import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.image.Image
import javafx.scene.image.ImageView
import javafx.scene.layout.BorderPane
import javafx.scene.layout.HBox
import javafx.scene.paint.ImagePattern
import javafx.scene.shape.Circle
import heast.client.model.Current
import heast.client.view.utility.FlexExpander
import heast.client.view.utility.FontManager

object MenubarView : HBox() {
	init {
		this.padding = Insets(10.0, 20.0, 10.0, 20.0)
		this.spacing = 40.0
		this.prefHeight = 60.0
		this.alignment = Pos.CENTER_RIGHT
		this.children.addAll(
			HBox(
				FontManager.boldLabel(size = 24.0).apply {
					this.textProperty().bind(Current.panel)
				},
				// search bar
			).apply {
				this.alignment = Pos.CENTER
				this.spacing = 10.0
			},

			FlexExpander(
				hBox = true
			),

			HBox(
				Circle(15.0).apply {
					BorderPane.setAlignment(this, Pos.TOP_CENTER)
					this.fill = ImagePattern(
						Image("/heast/client/images/avatars/default.png")
					)
				},
				FontManager.regularLabel("Heinz"),
				BorderPane(
					ImageView(
						Image("/heast/client/images/misc/expand.png")
					).apply {
						this.opacity = 0.5
						this.fitWidth = 20.0
						this.fitHeight = 20.0
					},
				)
			).apply {
				this.alignment = Pos.CENTER
				this.spacing = 10.0
			}
		)
	}
}