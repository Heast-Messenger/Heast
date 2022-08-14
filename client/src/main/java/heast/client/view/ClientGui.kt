package heast.client.view

import javafx.application.Application
import javafx.event.EventHandler
import javafx.scene.Scene
import javafx.scene.image.Image
import javafx.stage.Stage
import heast.client.control.network.ClientNetwork
import heast.client.model.Internal
import heast.client.model.Settings

object ClientGui {
	fun initialize() {
		Thread {
			println("Initializing graphical user interface...")
			Application.launch(
				App::class.java
			)
		}.start()
	}

	fun resize(to: Double) {
		val window = App.stage.scene.window
		val y = window.y
		val h = window.height
		window.height = to
		window.y = y + (h - to) / 2.0
	}

	class App : Application() {
		companion object {
			lateinit var stage: Stage

			fun welcome(s: Stage) {
				stage = s.apply {
					this.titleProperty().bind(Internal.welcomeTitle)
					this.scene = Scene(WelcomeView, 400.0, 450.0)
					this.isResizable = false
					this.show()
				}
			}

			fun chatting(s: Stage) {
				stage = s.apply {
					this.centerOnScreen() // BUG: Not working
					this.isResizable = true
					this.titleProperty().bind(
						Internal.mainTitle.concat(" - ").concat(Internal.titleQuote)
					)
					this.scene = Scene(MainView, 1100.0, 600.0).apply {
						this.stylesheets.add(
							"heast/client/css/style.css"
						)
					}
					this.show()
				}
			}
		}

		/**
		 * The entry point to the client interface.
		 */
		override fun start(s: Stage) {
			s.apply {
				this.icons.add(Image("/heast/client/images/logo/heast-rounded.png"))
			}

			Settings.account.addListener { _, _, newValue ->
				when (newValue) {
					null -> welcome(s)
					else -> chatting(s)
				}
			}

		    welcome(s)
//			chatting(s)

			s.onCloseRequest = EventHandler {
				ClientNetwork.INSTANCE.shutdown()
			}
		}
	}
}