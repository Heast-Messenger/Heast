package heast.client.gui.dialog

import javafx.beans.binding.Bindings
import javafx.event.EventHandler
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Cursor
import javafx.scene.image.Image
import javafx.scene.image.ImageView
import javafx.scene.layout.*
import heast.client.network.ClientNetwork
import heast.client.model.Settings
import heast.client.gui.MainView
import heast.client.gui.template.Button
import heast.client.gui.template.PortField
import heast.client.gui.template.TextField
import heast.client.gui.utility.FlexExpander
import heast.client.gui.utility.FontManager

object ServerAddDialog : BorderPane() {
	private val ipInput: TextField
	private val portInput: TextField

	init {
		this.padding = Insets(50.0)
		this.top = HBox(
			FontManager.boldLabel(
				"Add a server", 20.0
			),

			FlexExpander(
				hBox = true
			),

			BorderPane(
				ImageView(
					Image("heast/client/images/misc/close.png")
				).apply {
					this.fitHeight = 24.0
					this.fitWidth = 24.0
				}
			).apply {
				this.cursor = Cursor.HAND
				this.onMouseClicked = EventHandler {
					Dialog.close(this@ServerAddDialog, MainView.stackPane)
				}
			}
		).apply {
			this.padding = Insets(10.0)
			this.spacing = 10.0
			this.alignment = Pos.CENTER
		}

		this.center = VBox(
			TextField("IP Address").apply {
				ipInput = this
			},
			PortField("Port").apply {
				portInput = this
			},
			HBox(
				Button("Confirm", icon = Image(
					"heast/client/images/misc/confirm.png"
				)) {
					if (ipInput.text.isNotBlank() && portInput.text.isNotBlank()) {
						ClientNetwork.tryAddServer(
							ipInput.text.trim(),
							portInput.text.toInt()
						)
					}
					Dialog.close(this, MainView.stackPane)
				}.apply {
					HBox.setHgrow(this, Priority.ALWAYS)
					this.alignment = Pos.CENTER
				},

				Button("Test Connection", icon = Image(
					"heast/client/images/misc/test-connection.png"
				)) {
					if (ipInput.text.isNotBlank() && portInput.text.isNotBlank()) {
						ClientNetwork.testConnection(
							ipInput.text.trim(),
							portInput.text.toInt()
						)
					}
				}.apply {
					HBox.setHgrow(this, Priority.ALWAYS)
					this.alignment = Pos.CENTER
				}
			).apply {
				this.spacing = 10.0
			}
		).apply {
			this.padding = Insets(10.0, 50.0, 50.0, 50.0)
			this.spacing = 10.0
			this.alignment = Pos.CENTER
		}

		this.backgroundProperty().bind(
			Bindings.createObjectBinding({
				Background(
					BackgroundFill(
						Settings.colors["Secondary Color"]!!.color.value,
						CornerRadii(10.0),
						Insets(40.0),
					)
				)
			}, Settings.colors["Secondary Color"]!!.color)
		)
	}
}