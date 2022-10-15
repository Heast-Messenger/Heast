package heast.client.gui.sidebar

import javafx.beans.binding.Bindings
import javafx.event.EventHandler
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Cursor
import javafx.scene.control.ListCell
import javafx.scene.control.ListView
import javafx.scene.control.ScrollPane
import javafx.scene.image.Image
import javafx.scene.image.ImageView
import javafx.scene.layout.*
import javafx.util.Callback
import heast.client.model.Current
import heast.client.model.Settings.ServerListItem
import heast.client.model.Settings
import heast.client.gui.MainView
import heast.client.gui.dialog.Dialog
import heast.client.gui.dialog.ServerAddDialog
import heast.client.gui.template.Button
import heast.client.gui.utility.FontManager
import heast.client.gui.utility.HoverTransition

object ServerSidebarView : VBox() {
	init {
		this.spacing = 10.0
		this.children.addAll(
			ScrollPane(
				ListView<ServerListItem>().apply {
					this.items = Settings.servers
					this.cellFactory = Callback {
						ServerListCell()
					}
					this.selectionModel.selectedItemProperty().addListener { _, _, v ->
						if (v != null) {
							Current.server.set(v)
							Settings.servers.forEach { server -> server.setActive(false) }
							v.setActive(true)
						}
					}
				},
			).apply {
				setVgrow(this, Priority.ALWAYS)
				this.isFitToWidth = true
				this.backgroundProperty().bind(
					Bindings.createObjectBinding({
						Background(
							BackgroundFill(
								Settings.colors["Secondary Color"]!!.color.value,
								CornerRadii.EMPTY,
								Insets.EMPTY
							)
						)
					}, Settings.colors["Secondary Color"]!!.color)
				)
			},

			Button("Add server", icon = Image(
				"/heast/client/images/misc/add.png"
			)) {
				Dialog.show(ServerAddDialog, MainView.stackPane)
			}.apply {
				this.alignment = Pos.CENTER
			}
		)

		Settings.servers.addAll(
			ServerListItem(false, "Other Server", "other.example.com", 8080),
			ServerListItem(false, "My Server", "localhost", 25565),
			ServerListItem(false, "Friend's Server", "friend.example.com", 1982),
		)
	}

	class ServerListCell : ListCell<ServerListItem>() {
		override fun updateItem(item : ServerListItem?, empty : Boolean) {
			super.updateItem(item, empty)
			if (item != null) {
				graphic = HBox(
					ImageView(
						Image("/heast/client/images/settings/heast.png")
					).apply {
						this.fitHeight = 25.0
						this.fitWidth = 25.0
					},

					VBox(
						FontManager.boldLabel(
							item.name
						),
						FontManager.regularLabel(
							item.ip + " : " + item.port
						).apply {
							this.font = FontManager.regularFont(11.0)
						},
					)
				).apply {
					this.cursor = Cursor.HAND
					this.alignment = Pos.CENTER_LEFT
					this.spacing = 10.0
					this.padding = Insets(15.0)
					this.borderProperty().bind(
						Bindings.createObjectBinding({
							Border(
								BorderStroke(
									Settings.colors["Selection Color"]!!.color.value,
									BorderStrokeStyle.SOLID,
									CornerRadii(10.0),
									BorderWidths(1.0)
								)
							)
						}, Settings.colors["Selection Color"]!!.color)
					)
					this.backgroundProperty().bind(
						Bindings.createObjectBinding({
							Background(
								BackgroundFill(
									if (item.activeProperty.value) {
										Settings.colors["Selection Color"]!!.color.value
									} else {
										Settings.colors["Secondary Color"]!!.color.value
									},
									CornerRadii(10.0),
									Insets.EMPTY
								)
							)
						},
							Settings.colors["Selection Color"]!!.color,
							Settings.colors["Secondary Color"]!!.color,
							item.activeProperty
						)
					)

					this.onMouseEntered = EventHandler {
						HoverTransition.onMouseEntered(this@ServerListCell)
					}

					this.onMouseExited = EventHandler {
						HoverTransition.onMouseExited(this@ServerListCell)
					}
				}
			}
		}
	}
}