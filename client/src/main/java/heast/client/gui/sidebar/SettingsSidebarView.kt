package heast.client.gui.sidebar

import javafx.beans.binding.Bindings
import javafx.event.EventHandler
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Cursor
import javafx.scene.control.ListCell
import javafx.scene.control.ListView
import javafx.scene.control.ScrollPane
import javafx.scene.layout.*
import javafx.util.Callback
import heast.client.model.Internal
import heast.client.model.Internal.SettingsListItem
import heast.client.model.Settings
import heast.client.gui.SettingsView
import heast.client.gui.utility.FontManager
import heast.client.gui.utility.HoverTransition

object SettingsSidebarView : VBox() {
	init {
		this.spacing = 10.0
		this.children.addAll(
			ScrollPane(
				ListView<SettingsListItem>().apply {
					this.items = Internal.settingGroups
					this.cellFactory = Callback {
						SettingGroupCell()
					}
					this.selectionModel.selectedItemProperty().addListener { _, _, v ->
						if (v != null) {
							SettingsView.setContent(v.content)
							Internal.settingGroups.forEach { setting -> setting.setActive(false) }
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
			}
		)
	}

	class SettingGroupCell : ListCell<SettingsListItem>() {
		override fun updateItem(item : SettingsListItem?, empty : Boolean) {
			super.updateItem(item, empty)
			if (item != null) {
				graphic = HBox(
					FontManager.boldLabel(
						item.group
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
						HoverTransition.onMouseEntered(this@SettingGroupCell)
					}

					this.onMouseExited = EventHandler {
						HoverTransition.onMouseExited(this@SettingGroupCell)
					}
				}
			}
		}
	}
}