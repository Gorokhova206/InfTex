-- Function: public.add_folder(character, integer)

-- DROP FUNCTION public.add_folder(character, integer);

CREATE OR REPLACE FUNCTION public.add_folder(
    character,
    integer DEFAULT NULL::integer)
  RETURNS void AS
$BODY$
BEGIN
	begin 
		insert into folder(name,parent_id) values($1, $2);
		raise notice 'Новая папка добавлена';
	exception when others then
		raise notice 'Новая папка не добавлена';
	end;
END;
 
 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION public.add_folder(character, integer)
  OWNER TO postgres;
