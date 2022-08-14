package heast.client.view

import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Node
import javafx.scene.control.Label
import javafx.scene.image.Image
import javafx.scene.image.ImageView
import javafx.scene.layout.BorderPane
import javafx.scene.layout.VBox
import javafx.scene.paint.ImagePattern
import javafx.scene.shape.Circle
import heast.client.view.template.ViewPane
import heast.client.view.utility.FlexSpacer
import heast.client.view.utility.FontManager
import java.nio.file.Path
import kotlin.io.path.extension
import kotlin.io.path.name

object ChatView : ViewPane() {
	init {
		this.center = ChatPane
	}

	object ChatPane : VBox() {
		init {
			addMessage(0, "Servus!")
		}

		fun addSpacing() {
			this.children.add(
				FlexSpacer(
					10.0, vBox = true
				)
			)
		}

		fun addMessage(userID: Int, message: String) {
			this.children.add(
				ChatItem(userID, FontManager.regularLabel(message).apply {
					this.isWrapText = true
				})
			)
		}

		fun addImage(userID: Int, image: Image) {
			this.children.add(
				ChatItem(userID, ImageView(image).apply {
					this.fitWidth = 400.0
					this.fitHeight = 400.0 * image.height / image.width
				})
			)
		}

		fun addFile(userID: Int, file: Path) {
			this.children.add(
				ChatItem(userID, BorderPane().apply {
					this.left = ImageView(Image(
						file.extension.let {
							when (it) {
								"png" -> "/heast/client/images/files/image.png"
								"jpg" -> "/heast/client/images/files/image.png"
								"jpeg" -> "/heast/client/images/files/image.png"
								"gif" -> "/heast/client/images/files/image.png"
								"bmp" -> "/heast/client/images/files/image.png"
								"tiff" -> "/heast/client/images/files/image.png"
								"svg" -> "/heast/client/images/files/image.png"
								"webp" -> "/heast/client/images/files/image.png"

								"mp3" -> "/heast/client/images/files/audio.png"
								"wav" -> "/heast/client/images/files/audio.png"
								"flac" -> "/heast/client/images/files/audio.png"
								"aac" -> "/heast/client/images/files/audio.png"
								"ogg" -> "/heast/client/images/files/audio.png"

								"mp4" -> "/heast/client/images/files/movie.png"
								"avi" -> "/heast/client/images/files/movie.png"
								"mkv" -> "/heast/client/images/files/movie.png"
								"mov" -> "/heast/client/images/files/movie.png"
								"flv" -> "/heast/client/images/files/movie.png"

								"pdf" -> "/heast/client/images/files/pdf.png"

								"zip" -> "/heast/client/images/files/zip.png"
								"rar" -> "/heast/client/images/files/zip.png"
								"7z" -> "/heast/client/images/files/zip.png"
								"tar" -> "/heast/client/images/files/zip.png"
								"gz" -> "/heast/client/images/files/zip.png"
								"bz2" -> "/heast/client/images/files/zip.png"

								else -> "/heast/client/images/files/file.png"
							}
						}
					)).apply {
						this.fitWidth = 20.0
						this.fitHeight = 20.0
					}

					this.center = Label(file.name).apply {
						setAlignment(this, Pos.CENTER_LEFT)
						this.padding = Insets(0.0, 10.0, 0.0, 10.0)
					}

					this.right = ImageView(
						Image(
							"/heast/client/images/files/download.png"
						)
					).apply {
						this.fitWidth = 20.0
						this.fitHeight = 20.0
					}
				})
			)
		}
	}

	class ChatItem(userID: Int, element: Node) : BorderPane() {
		init {
			this.padding = Insets(10.0)
			this.left = Circle(15.0).apply {
				setAlignment(this, Pos.TOP_CENTER)
				this.fill = ImagePattern(
					Image(
						"/heast/client/images/avatars/default.png"
					)
				)
			}

			this.center = VBox(
				FontManager.boldLabel("Heinz").apply {
					this.isWrapText = false
					this.prefHeight = 30.0
				}, element
			).apply {
				this.padding = Insets(0.0, 0.0, 0.0, 15.0)
			}
		}
	}
}