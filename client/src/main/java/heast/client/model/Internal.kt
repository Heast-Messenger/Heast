package heast.client.model

import AccountArea
import javafx.beans.property.SimpleBooleanProperty
import javafx.beans.property.SimpleStringProperty
import javafx.collections.FXCollections
import javafx.collections.ObservableList
import javafx.scene.Node
import heast.client.gui.settings.AppearanceArea
import heast.client.gui.settings.NetworkArea

object Internal {
	val version = SimpleStringProperty(
		"0.1.0"
	)

    val mainTitle = SimpleStringProperty(
		"Whörld Wide Messenger"
	)

	val welcomeTitle = SimpleStringProperty(
		"Welcome"
	)

	val titleQuote = SimpleStringProperty(
		"The messenger with revolutionary technology"
	)

	val settingGroups : ObservableList<SettingsListItem> = FXCollections.observableList(
		listOf(
			SettingsListItem(group = "Account", content = AccountArea),
			SettingsListItem(group = "Appearance", content = AppearanceArea),
			SettingsListItem(group = "Network", content = NetworkArea),
		)
	)

	class SettingsListItem(active : Boolean = false, val group : String, val content: Node) {
		val activeProperty = SimpleBooleanProperty(active)

		fun setActive(active : Boolean) {
			activeProperty.set(active)
		}
	}
}