package heast.client.view

import javafx.event.EventHandler
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Cursor
import javafx.scene.effect.ColorAdjust
import javafx.scene.image.Image
import javafx.scene.image.ImageView
import javafx.scene.layout.BorderPane
import javafx.scene.layout.HBox
import javafx.scene.layout.VBox
import heast.client.view.template.ViewPane
import heast.client.view.utility.FlexSpacer
import heast.client.view.utility.FontManager
import java.awt.Desktop
import java.net.URI

object HomeView : ViewPane() {
	init {
		this.center = HomePane
	}

	object HomePane : HBox() {
		init {
			this.opacity = 0.3
			this.children.addAll(
				VBox(
					FontManager.boldLabel("Heast Messenger", 30.0),
					FlexSpacer(10.0, vBox = true),
					FontManager.regularLabel("The solution for your daily messenger.", 20.0).apply {
						this.isWrapText = true
					},
					FontManager.regularLabel("Simple, clean, and intuitive.", 20.0).apply {
						this.isWrapText = true
					},
					FontManager.regularLabel("Open Source with end-to-end encryption.", 20.0).apply {
						this.isWrapText = true
					},
					FlexSpacer(10.0, vBox = true),
					FontManager.regularLabel("Support us on social media:", 20.0).apply {
						this.isWrapText = true
					},
					FlexSpacer(20.0, vBox = true),
					HBox(
						SocialLink(
							"GitHub", Image(
								"/heast/client/images/social/github.png",
							), URI("https://github.com/m-ue-d/Wh-oe-rldWideMessenger")
						),

						SocialLink(
							"Reddit", Image(
								"/heast/client/images/social/reddit.png",
							), URI("")
						),

						SocialLink(
							"Twitter", Image(
								"/heast/client/images/social/twitter.png",
							), URI("")
						)
					).apply {
						this.alignment = Pos.CENTER_LEFT
						this.spacing = 20.0
					}
				).apply {
					this.spacing = 10.0
					this.prefWidth = 400.0
					this.alignment = Pos.CENTER_LEFT
				},

				ImageView(
					Image("/heast/client/images/logo/heast-grayscale.png")
				).apply {
//					this.opacity = 0.4
					this.fitWidth = 400.0
					this.fitHeight = 400.0
				}
			)

			this.alignment = Pos.CENTER
			this.spacing = 50.0
		}
	}

	class SocialLink(name: String, img: Image, link: URI) : BorderPane() {
		init {
			this.cursor = Cursor.HAND
			this.center = ImageView(img).apply {
				this.fitWidth = 30.0
				this.fitHeight = 30.0
			}
			this.bottom = FontManager.boldLabel(name, 13.0).apply {
				this.isWrapText = true
				this.padding = Insets(5.0)
			}
			this.onMouseClicked = EventHandler {
				Desktop.getDesktop().browse(link)
			}
		}
	}
}