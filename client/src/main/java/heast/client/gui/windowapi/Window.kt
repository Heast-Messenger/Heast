package heast.client.gui.windowapi

import com.jetbrains.JBR
import heast.client.ClientResources
import heast.client.gui.registry.Fonts
import heast.client.gui.registry.Colors.toAWT
import heast.core.logging.IO
import heast.core.utility.OS
import javafx.application.Platform
import javafx.embed.swing.JFXPanel
import javafx.scene.Parent
import javafx.scene.Scene
import javafx.scene.paint.Color
import java.awt.Dimension
import javax.swing.JFrame
import javax.swing.SwingUtilities
import kotlin.reflect.KClass

class Window {
	var title: String = "Window"
	var width: Int = 500
	var height: Int = 500
	var background: Color = Color.WHITE
	var isResizable: Boolean = true
	var draggableBody: Boolean = false
	var isTitleBarHidden: Boolean = false
	var isFullWindowContent: Boolean = false
	var isWindowTitleVisible: Boolean = false
	var isAWTManager: Boolean = false
	var titleBarHeight: Int = 30

	fun withTitle(title: String) = apply { this.title = title }
	fun withWidth(width: Int) = apply { this.width = width }
	fun withHeight(height: Int) = apply { this.height = height }
	fun isResizable(resizable: Boolean) = apply { this.isResizable = resizable }
	fun withBackground(background: Color) = apply { this.background = background }
	fun isDraggableBody(draggableBody: Boolean) = apply { this.draggableBody = draggableBody }
	fun isTitleBarHidden(isTitleBarHidden: Boolean) = apply { this.isTitleBarHidden = isTitleBarHidden }
	fun isFullWindowContent(isFullWindowContent: Boolean) = apply { this.isFullWindowContent = isFullWindowContent }
	fun isWindowTitleVisible(isWindowTitleVisible: Boolean) = apply { this.isWindowTitleVisible = isWindowTitleVisible }
	fun isAWTManager(isAWTManager: Boolean) = apply { this.isAWTManager = isAWTManager }
	fun withTitleBarHeight(titleBarHeight: Int) = apply { this.titleBarHeight = titleBarHeight }

	lateinit var frame: JFrame
	lateinit var jfxPanel: JFXPanel
	lateinit var mantle : Mantle

	init { initProperties() }

	@JvmName("setWindowHeight")
	fun setHeight(height: Int) {
		this.height = height
		jfxPanel.size = Dimension(width, height)
		SwingUtilities.invokeLater {
			frame.size = Dimension(width, height)
		}
	}

	private fun initWin(frame: JFrame) {
		initJBR()
	}

	private fun initOSX(frame: JFrame) {
		if (OS.isMac() && isAWTManager) {
			frame.rootPane.putClientProperty("apple.awt.fullWindowContent", isFullWindowContent)
			frame.rootPane.putClientProperty("apple.awt.transparentTitleBar", isTitleBarHidden)
			frame.rootPane.putClientProperty("apple.awt.draggableWindowBackground", draggableBody)
			frame.rootPane.putClientProperty("apple.awt.windowTitleVisible", isWindowTitleVisible)
			frame.rootPane.putClientProperty("apple.awt.fullscreenable", isResizable)
		} else {
			initJBR()
		}
	}

	private fun initJBR() {
		if (JBR.isAvailable()) {
			if (JBR.isCustomWindowDecorationSupported()) {
				JBR.getCustomWindowDecoration().setCustomDecorationEnabled(frame, true)
				JBR.getCustomWindowDecoration().setCustomDecorationTitleBarHeight(frame, titleBarHeight)
//				JBR.getCustomWindowDecoration().setCustomDecorationHitTestSpots(frame, listOf(
//					mapOf(Rectangle(width - 60, 0, 60, titleBarHeight) to CustomWindowDecoration.OTHER_HIT_SPOT).entries.first()
//				))
			}
			if (JBR.isRoundedCornersManagerSupported()) {
				JBR.getRoundedCornersManager().setRoundedCorners(frame, "full")
			}
		}
	}

	private fun initProperties() {
		if (OS.isMac()) {
			System.setProperty("apple.awt.application.name", title)
			System.setProperty("apple.awt.application.appearance", "system")
			System.setProperty( "apple.awt.fullWindowContent", isFullWindowContent.toString() )
			System.setProperty( "apple.awt.transparentTitleBar", isTitleBarHidden.toString() )
		}
	}

	fun <T> build(content : KClass<T>) : Window where T : Parent {
		SwingUtilities.invokeLater {
			frame = JFrame(title)
			initOSX(frame)
			initWin(frame)

			jfxPanel = JFXPanel()
			frame.contentPane.background = this@Window.background.toAWT()
			frame.isResizable = isResizable
			frame.add(jfxPanel)
			frame.size = Dimension(width, height)
			frame.defaultCloseOperation = JFrame.EXIT_ON_CLOSE

			Platform.runLater {
				initFX(content)
			}
		}
		return this@Window
	}

	private fun initFX(content: KClass<out Parent>) {
		Fonts.init()
		jfxPanel.scene = Scene(
			Mantle(content.objectInstance!!).apply { mantle = this },
			this@Window.width.toDouble(),
			this@Window.height.toDouble(),
			this@Window.background
		).apply {
			this.fill = this@Window.background
			initCss(this)
			SwingUtilities.invokeLater {
				frame.isVisible = true }
		}
	}

	private fun initCss(scene: Scene) {
		scene.stylesheets.clear()
		scene.userAgentStylesheet

		ClientResources.getResourceFile("css").listFiles { file -> file.extension == "css" }?.forEach { file ->
//			var content = file.readText()
//			Colors.colors().forEach { c ->
//				content = content.replace("\$${c.key}", c.value.toHex())
//			}
//			file.writeText(content)
			scene.stylesheets.add(file.toURI().toURL().toExternalForm())
			IO.info.println("Loaded CSS: ${file.name} - ${file.absolutePath}")
		}
	}
}