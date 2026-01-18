import { NtsTinymceOptions } from './nts-tinymce.config.interface';
export class NtsTinymceDefaultOptions implements NtsTinymceOptions {
	plugins = [
		'link', 
		'paste', 
		'table', 
		'advlist', 
		'autoresize', 
		'lists',
		'code'
	];
	skin_url = '/assets/tinymce/skins/ui/oxide';
	baseURL = '/assets/tinymce';
	auto_focus = true;
}