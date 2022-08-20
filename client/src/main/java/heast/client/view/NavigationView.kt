package heast.client.view

import javafx.animation.FadeTransition
import javafx.animation.RotateTransition
import javafx.animation.ScaleTransition
import javafx.beans.binding.Bindings
import javafx.beans.property.SimpleBooleanProperty
import javafx.event.EventHandler
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Cursor
import javafx.scene.Node
import javafx.scene.image.Image
import javafx.scene.image.ImageView
import javafx.scene.input.MouseEvent
import javafx.scene.layout.*
import javafx.scene.paint.Color
import javafx.util.Duration
import heast.client.model.Current
import heast.client.model.Settings
import heast.client.view.sidebar.ServerSidebarView
import heast.client.view.sidebar.SettingsSidebarView
import heast.client.view.template.ViewPane
import heast.client.view.utility.FlexExpander
import heast.client.view.utility.FlexSpacer
import heast.client.view.utility.Interpolator
import java.util.*

object NavigationView : VBox() {
	val toggleGroup = ToggleGroup()
	init {
		this.padding = Insets(10.0, 0.0, 20.0, 0.0)
		this.spacing = 20.0
		this.prefWidth = 50.0
		this.alignment = Pos.TOP_CENTER
		this.children.addAll(
			SidebarButton(
				Image("/heast/client/images/logo/messenger-hollow.png"), HomeView, toggleGroup
			) {
				SidebarView.hide()
				Current.panel.set("Welcome!")
			}.apply {
				this.active.set(true)
			},

			FlexSpacer(
				20.0, vBox = true
			),

			SidebarButton(
				Image("/heast/client/images/menu/messages.png"), ChatView, toggleGroup
			) {
				SidebarView.show(ViewPane())
				Current.panel.set("Direct Messages")
			},
			SidebarButton(
				Image("/heast/client/images/settings/client.png"), ViewPane(), toggleGroup
			) {
				SidebarView.hide()
				Current.panel.set("Friends")
			},

			FlexSpacer(
				20.0, vBox = true
			),

			SidebarButton(
				Image("/heast/client/images/menu/servers.png"), ChatView, toggleGroup
			) {
				SidebarView.show(ServerSidebarView)
				Current.panel.set("Servers")
			},
			SidebarButton(
				Image("/heast/client/images/menu/explore.png"), ViewPane(), toggleGroup
			) {
				SidebarView.hide()
				Current.panel.set("Discover")
			},

			FlexExpander(
				vBox = true
			),

			SidebarButton(
				Image("/heast/client/images/menu/settings.png"), SettingsView, toggleGroup
			) {
				SidebarView.show(SettingsSidebarView, 200.0)
				Current.panel.set("Settings")
			},
		)
	}

	class ToggleGroup {
		private val buttons = mutableListOf<SidebarButton>()
		fun add(button: SidebarButton) {
			buttons.add(button)
		}
		fun toggle(button: SidebarButton) {
			for (b in buttons) {
				if (b == button) {
					b.active.set(true)
				} else {
					b.active.set(false)
				}
			}
		}
		fun reset() {
			if(buttons.size>0){
				for(b in buttons) {
					b.active.set(false)
				}
				buttons[0].active.set(true)
			}
		}
	}

	class SidebarButton(private val img: Image, private val content: Node, private val toggleGroup : ToggleGroup? = null, private val onAction: EventHandler<MouseEvent>? = null) : BorderPane() {
		val active: SimpleBooleanProperty = SimpleBooleanProperty(false)

		init {
			toggleGroup?.add(this)
			this.cursor = Cursor.HAND
			this.prefHeight = 40.0
			this.center = ImageView(img).apply {
				this.fitWidth = 15.0
				this.fitHeight = 15.0
				this.opacity = 0.5
			}

			active.addListener { _, _, v ->
				if (v && toggleGroup != null) {
					FadeTransition().apply {
						this.node = center
						this.fromValue = 0.5
						this.toValue = 1.0
						this.duration = Duration.seconds(0.4)
						this.interpolator = Interpolator.easeInOutBack
					}.play()
					this@SidebarButton.borderProperty().bind(
						Bindings.createObjectBinding({
							Border(
								BorderStroke(
									Color.TRANSPARENT,
									Color.TRANSPARENT,
									Color.TRANSPARENT,
									Settings.colors["Accent Color"]!!.color.value,
									BorderStrokeStyle.NONE,
									BorderStrokeStyle.NONE,
									BorderStrokeStyle.NONE,
									BorderStrokeStyle.SOLID,
									CornerRadii.EMPTY,
									BorderWidths(4.0),
									Insets.EMPTY,
								)
							)
						}, Settings.colors["Accent Color"]!!.color)
					)
				} else {
					FadeTransition().apply {
						this.node = center
						this.fromValue = 1.0
						this.toValue = 0.5
						this.duration = Duration.seconds(0.4)
						this.interpolator = Interpolator.easeInOutBack
					}.play()
					this@SidebarButton.borderProperty().unbind()
					this@SidebarButton.border = null
				}
			}

			this.onMouseEntered = EventHandler {
				ScaleTransition().apply {
					this.node = center
					this.fromX = 1.0
					this.fromY = 1.0
					this.toX = 1.3
					this.toY = 1.3
					this.duration = Duration.seconds(0.5)
					this.interpolator = Interpolator.easeInOutBack
				}.play()
				RotateTransition().apply {
					this.node = center
					this.toAngle = Random().nextDouble() * 20.0 - 10.0
					this.duration = Duration.seconds(0.3)
					this.interpolator = Interpolator.easeOut
				}.play()
			}
			this.onMouseExited = EventHandler {
				ScaleTransition().apply {
					this.node = center
					this.fromX = 1.3
					this.fromY = 1.3
					this.toX = 1.0
					this.toY = 1.0
					this.duration = Duration.seconds(0.5)
					this.interpolator = Interpolator.easeInOutBack
				}.play()
				RotateTransition().apply {
					this.node = center
					this.toAngle = 0.0
					this.duration = Duration.seconds(0.3)
					this.interpolator = Interpolator.easeOut
				}.play()
			}
			this.onMousePressed = EventHandler {
				if (!active.value) {
					MainView.setView(content)
					onAction?.handle(it)
				}
				active.set(true)
				toggleGroup?.toggle(this@SidebarButton)
				ScaleTransition().apply {
					this.node = center
					this.fromX = 1.3
					this.fromY = 1.3
					this.toX = 1.0
					this.toY = 1.0
					this.duration = Duration.seconds(0.5)
					this.interpolator = Interpolator.easeInOutBack
				}.play()
			}
			this.onMouseReleased = EventHandler {
				ScaleTransition().apply {
					this.node = center
					this.fromX = 1.0
					this.fromY = 1.0
					this.toX = 1.3
					this.toY = 1.3
					this.duration = Duration.seconds(0.5)
					this.interpolator = Interpolator.easeInOutBack
				}.play()
			}
		}
	}
}