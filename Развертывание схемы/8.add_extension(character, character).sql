-- Function: public.add_extension(character, character)

-- DROP FUNCTION public.add_extension(character, character);

CREATE OR REPLACE FUNCTION public.add_extension(
    character,
    character)
  RETURNS void AS
$BODY$
BEGIN
	begin
		insert into extension(type, image)
		values($1, $2);
		raise notice 'Добавлено';
	exception when others then
		raise notice 'Не добавлено';
	end;
END;
 
 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION public.add_extension(character, character)
  OWNER TO postgres;
