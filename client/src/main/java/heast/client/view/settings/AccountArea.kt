import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.image.Image
import javafx.scene.layout.*
import javafx.scene.paint.Color
import javafx.scene.paint.ImagePattern
import javafx.scene.shape.Circle
import heast.client.control.network.ClientNetwork
import heast.client.model.Settings
import heast.client.view.SettingsView
import heast.client.view.template.Button
import heast.client.view.utility.FlexExpander
import heast.client.view.utility.FlexItem
import heast.client.view.utility.FlexSpacer
import heast.client.view.utility.FontManager
import java.time.format.DateTimeFormatter
import kotlin.math.min

object AccountArea : VBox() {
	init {
		this.spacing = 10.0
		this.children.addAll(
			SettingsView.SettingsGroup("Account"),
			HBox(
				VBox(
					HBox(
						Circle(75.0).apply {
							this.fillProperty().bind(
								Bindings.createObjectBinding({
									return@createObjectBinding ImagePattern(
										// Image(Settings.account.value?.img ?: "/path/to/default/img.png")
										Image("/heast/client/images/avatars/default.png")
									)
								}, Settings.account)
							)
						},
						VBox(
							FontManager.boldLabel("", 20.0).apply {
								this.textProperty().bind(
									Bindings.createObjectBinding({
										return@createObjectBinding Settings.account.value?.username
											?: "Not logged in"
									}, Settings.account)
								)
							},

							FlexItem(vBox = true),

							HBox(
								FontManager.regularLabel("email: ", 16.0),
								FontManager.boldLabel("", 16.0).apply {
									this.textProperty().bind(
										Bindings.createObjectBinding({
											val email = Settings.account.value?.email
												?: "guest@wwm"
											return@createObjectBinding "${
												email.substring(0..4)
											}[...]${
												email.substring(email.length - 4)
											}"
										}, Settings.account)
									)
								}
							),

							HBox(
								FontManager.regularLabel("password: ", 16.0),
								FontManager.boldLabel("**********", 16.0)
							),

							HBox(
								FontManager.regularLabel("member since: ", 16.0),
								FontManager.boldLabel("", 16.0).apply {
									this.textProperty().bind(
										Bindings.createObjectBinding({
											return@createObjectBinding Settings.account.value?.since?.format(
												DateTimeFormatter.ofPattern("dd. MMM. yyyy")
											) ?: "Unknown"
										}, Settings.account)
									)
								}
							)
						).apply {
							this.padding = Insets(20.0)
							this.spacing = 10.0
						}
					).apply {
						this.alignment = Pos.CENTER_LEFT
					},

					VBox(
						FontManager.regularLabel("Account Information:", 16.0).apply {
							this.isWrapText = true
						},

						FlexSpacer(5.0, vBox = true),

						HBox(
							FontManager.regularLabel(" • uid: ", 16.0),
							FontManager.boldLabel("", 16.0).apply {
								this.textProperty().bind(
									Bindings.createObjectBinding({
										return@createObjectBinding Settings.account.value?.id?.toString()
											?: "Unknown"
									}, Settings.account)
								)
							}
						),

						HBox(
							FontManager.regularLabel(" • key: ", 16.0),
							FontManager.boldLabel("", 16.0).apply {
								this.textProperty().bind(
									Bindings.createObjectBinding({
										val publicKey : String = Settings.account.value?.publicKey?.toString()
											?: "A very long number"
										return@createObjectBinding "${
											publicKey.substring(0..min(20, publicKey.length - 1))
										}..."
									}, Settings.account)
								)
							}
						),
					).apply {
						this.spacing = 5.0
						this.opacity = 0.3
					}
				).apply {
					this.spacing = 20.0
				},

				FlexExpander(
					hBox = true
				),

				VBox(
					Button("Log out", Color.web("#FF6F6F"), Image(
						"/heast/client/images/settings/logout.png"
					)) {
						ClientNetwork.INSTANCE.logout()
					},
					Button("Switch account", Color.web("#ECF0FF"), Image(
						"/heast/client/images/settings/switchacc.png"
					)) {

					},
					Button("Invite a friend", Color.web("#7CC0FF"), Image(
						"/heast/client/images/settings/invite.png"
					)) {

					},
					Button("Request account data", Color.web("#ECF0FF"), Image(
						"/heast/client/images/settings/accdata.png"
					)) {

					},
				).apply {
					this.spacing = 10.0
				}
			).apply {
				this.padding = Insets(10.0, 20.0, 10.0, 20.0)
			}
		)

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