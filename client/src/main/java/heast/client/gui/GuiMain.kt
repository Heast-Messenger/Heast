package heast.client.gui

import heast.client.gui.registry.Colors
import heast.client.gui.scenes.Start
import heast.client.gui.windowapi.Window

object GuiMain {

	lateinit var window : Window

	@JvmStatic
	fun initialize() {
		window = Window()
			.withBackground(Colors.PRIMARY)
			.withWidth(400)
			.withHeight(520)
			.isResizable(false)
			.isTitleBarHidden(true)
			.isFullWindowContent(true)
			.isWindowTitleVisible(false)
			.isDraggableBody(true)
			.isAWTManager(false)
			.withTitleBarHeight(60)
			.build(Start::class)
	}
}