package heast.client.model

import javafx.beans.Observable
import javafx.beans.property.SimpleBooleanProperty
import javafx.beans.property.SimpleIntegerProperty
import javafx.beans.property.SimpleObjectProperty
import javafx.beans.property.SimpleStringProperty
import javafx.collections.FXCollections
import javafx.collections.ObservableList
import javafx.scene.paint.Color
import heast.core.network.UserAccount

object Settings {

	/**
	 * The user account that is used to log in on startup
	 */
    val account = SimpleObjectProperty<UserAccount?>(
		null
	)

	/**
	 * A list of registered servers on this account. Updates to this list will be reflected in the listview.
	 */
	val servers : ObservableList<ServerListItem> = FXCollections.observableList( ArrayList() ) {
		param : ServerListItem -> arrayOf<Observable?>(
			param.nameProperty, param.ipProperty, param.portProperty
		)
	}

	class ServerListItem(active : Boolean, name : String, ip : String, port : Int) {
		val activeProperty = SimpleBooleanProperty(active)
		val nameProperty = SimpleStringProperty(name)
		val ipProperty = SimpleStringProperty(ip)
		val portProperty = SimpleIntegerProperty(port)

		fun setActive(active : Boolean) {
			activeProperty.set(active)
		}

		val name : String get() = nameProperty.get()
		val ip : String get() = ipProperty.get()
		val port : Int get() = portProperty.get()
	}

	/**
	 * The full color set used inside the application.
	 */
	val colors : Map<String, ColorItem> = mapOf(
		"Primary Color" to (SimpleObjectProperty( Color.web("#1d1d21") ) to
			"The primary color of the application. It is the darkest color of all."
		),
		"Secondary Color" to (SimpleObjectProperty( Color.web("#26262b") ) to
			"A brighter variant of the primary color. Used for elevated elements."
		),
		"Selection Color" to (SimpleObjectProperty( Color.web("#34343a") ) to
			"The color of selected items in a list."
		),
		"Tertiary Color" to (SimpleObjectProperty( Color.web("#61636a") ) to
			"The lightest color in the application. Used for top level elements."
		),
		"Accent Color" to (SimpleObjectProperty( Color.web("#0074f1") ) to
			"A colorful alternative to the usual palette. Mainly used for tab indicators."
		),
		"Font Color" to (SimpleObjectProperty( Color.web("#ebf2ff") ) to
			"The main color of the application's font. Usually set to a color close to white."
		)
	)

	data class ColorItem(
		val color : SimpleObjectProperty<Color>,
		val description : String
	)

	private infix fun SimpleObjectProperty<Color>.to(description : String) : ColorItem = ColorItem(
		this, description
	)
}