package heast.client.view

import AccountArea
import javafx.beans.binding.Bindings
import javafx.beans.property.SimpleDoubleProperty
import javafx.event.EventHandler
import javafx.geometry.Insets
import javafx.geometry.Orientation
import javafx.geometry.Pos
import javafx.scene.Node
import javafx.scene.control.ScrollPane
import javafx.scene.control.Separator
import javafx.scene.layout.*
import javafx.scene.paint.Color
import javafx.util.Duration
import heast.client.model.Internal
import heast.client.model.Settings
import heast.client.view.settings.AppearanceArea
import heast.client.view.settings.NetworkArea
import heast.client.view.template.ViewPane
import heast.client.view.utility.*
import kotlin.math.min

object SettingsView : ViewPane() {
	private val stackPane: StackPane

	init {
		this.center = ScrollPane(
			StackPane(
				// Content
			).apply {
				stackPane = this
			}
		).apply {
			this.maxWidth = 1300.0
			this.isFitToWidth = true
			this.background = Background.fill(Color.TRANSPARENT)

		}

		Internal.settingGroups[0].setActive(true)
		setContent(Internal.settingGroups[0].content)
	}

	fun setContent(content: Node) {
		MultiStack.setStackPaneView(content, stackPane)
	}

//	fun scrollToGroup(group : String) {
//		AnyTransition {
//			scrollPane.vvalue = it
//		}.apply {
//			this.duration = Duration.seconds(0.5)
//			this.interpolator = Interpolator.easeOut
//			this.from = scrollPane.vvalue
//			this.to = min( // Scroll element into view
//				groups[group]!!.boundsInParent.minY.div(
//					scrollPane.content.layoutBounds.height - scrollPane.height
//				), 1.0
//			)
//		}.play()
//	}

	class SettingsGroup(title: String) : HBox() {
		init {
			this.alignment = Pos.CENTER
			this.padding = Insets(20.0)
			this.spacing = 10.0
			this.children.addAll(
				Separator(Orientation.HORIZONTAL).apply {
					this.backgroundProperty().bind(
						Bindings.createObjectBinding({
							return@createObjectBinding Background.fill(
								Settings.colors["Tertiary Color"]!!.color.value
							)
						}, Settings.colors["Tertiary Color"]!!.color)
					)
					setHgrow(this, Priority.ALWAYS)
				},
				FontManager.boldLabel(title, 16.0).apply {
					this.textFillProperty().bind(
						Settings.colors["Tertiary Color"]!!.color
					)
				},
				Separator(Orientation.HORIZONTAL).apply {
					this.backgroundProperty().bind(
						Bindings.createObjectBinding({
							return@createObjectBinding Background.fill(
								Settings.colors["Tertiary Color"]!!.color.value
							)
						}, Settings.colors["Tertiary Color"]!!.color)
					)
					setHgrow(this, Priority.ALWAYS)
				}
			)
		}
	}

	class SettingsItem(title: String, description: String, setting: Region) : HBox() {
		private val hoverOpacity = SimpleDoubleProperty(0.0)

		init {
			this.alignment = Pos.CENTER
			this.padding = Insets(10.0, 20.0, 10.0, 20.0)
			this.spacing = 10.0
			this.children.addAll(
				VBox(
					FontManager.boldLabel(title, 16.0).apply {
						this.textFillProperty().bind(
							Settings.colors["Font Color"]!!.color
						)
					},
					FontManager.regularLabel(description, 14.0).apply {
						this.prefWidth = 300.0
						this.isWrapText = true
						this.textFillProperty().bind(
							Settings.colors["Tertiary Color"]!!.color
						)
					}
				).apply {
					this.spacing = 10.0
				},

				FlexExpander(
					hBox = true
				),

				setting.apply {
					this.prefHeight = 40.0
					this.border = null
					this.backgroundProperty().bind(
						Bindings.createObjectBinding({
							return@createObjectBinding Background(
								BackgroundFill(
									Settings.colors["Primary Color"]!!.color.value,
									CornerRadii(5.0),
									Insets.EMPTY
								)
							)
						}, Settings.colors["Primary Color"]!!.color)
					)
				}
			)

			this.backgroundProperty().bind(
				Bindings.createObjectBinding({
					return@createObjectBinding Background(
						BackgroundFill(
							Settings.colors["Tertiary Color"]!!.color.value.deriveColor(
								0.0, 1.0, 1.0, hoverOpacity.value
							),
							CornerRadii(7.0),
							Insets.EMPTY
						)
					)
				}, Settings.colors["Tertiary Color"]!!.color, hoverOpacity)
			)

			this.onMouseEntered = EventHandler {
				AnyTransition {
					hoverOpacity.set(it)
				}.apply {
					this.from = hoverOpacity.value
					this.to = 0.3
					this.duration = Duration.seconds(0.3)
					this.interpolator = Interpolator.easeOut
				}.play()
			}

			this.onMouseExited = EventHandler {
				AnyTransition {
					hoverOpacity.set(it)
				}.apply {
					this.from = hoverOpacity.value
					this.to = 0.0
					this.duration = Duration.seconds(0.3)
					this.interpolator = Interpolator.easeOut
				}.play()
			}
		}
	}
}